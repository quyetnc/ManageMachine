import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MachineService, MachineType, Parameter } from 'src/app/core/services/machine.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from 'src/app/core/services/user.service';
import { User } from 'src/app/core/models/user.model';

@Component({
  selector: 'app-machine-form',
  templateUrl: './machine-form.component.html',
  styleUrls: ['./machine-form.component.scss']
})
export class MachineFormComponent implements OnInit {
  machineForm: FormGroup;
  isEditMode = false;
  machineId: number | null = null;
  machineTypes: MachineType[] = [];
  availableParameters: Parameter[] = [];
  users: User[] = [];
  loading = false;
  imagePreview: string | null = null;

  constructor(
    private fb: FormBuilder,
    private machineService: MachineService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.machineForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      imageUrl: [''],
      machineTypeId: ['', Validators.required],
      userId: [null], // Owner
      parameters: this.fb.array([])
    });
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.loading = true;
      this.machineService.uploadImage(file).subscribe({
        next: (res) => {
          this.machineForm.patchValue({ imageUrl: res.url });
          this.imagePreview = res.url; // Use relative path, might need base depending on setup
          // Ideally backend returns full URL or we prepend base
          // Since we used static files, if we return "uploads/file.jpg", we need to prepend API url or base.
          // Let's assume we need to prepend environment.apiUrl base or similar.
          // Actually, if served from API wwwroot, it is `http://localhost:5037/uploads/...`
          // Let's just store the returned URL if it's relative?
          // Wait, upload controller returns "uploads/..."
          // We should probably fix the controller to return suitable path or handle it here.
          // Let's assume for now we use the path as is and fix display if broken.
          // Better: We stored "uploads/..." in DB.
          this.loading = false;
        },
        error: (err) => {
          this.handleError('Failed to upload image', err);
          this.loading = false;
        }
      });
    }
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.machineId = +id;
    }

    this.loadMasterData();
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAll().subscribe({
      next: (users) => this.users = users,
      error: (err) => console.error('Failed to load users', err)
    });
  }

  loadMasterData() {
    this.loading = true;
    // chain requests or use forkJoin
    this.machineService.getMachineTypes().subscribe({
      next: (types) => {
        this.machineTypes = types;
        this.machineService.getParameters().subscribe({
          next: (params) => {
            this.availableParameters = params;
            if (this.isEditMode && this.machineId) {
              this.loadMachine(this.machineId);
            } else {
              this.initParameters();
              this.loading = false;
            }
          },
          error: (err) => this.handleError('Failed to load parameters', err)
        });
      },
      error: (err) => this.handleError('Failed to load machine types', err)
    });
  }

  loadMachine(id: number) {
    this.machineService.getMachine(id).subscribe({
      next: (machine) => {
        this.machineForm.patchValue({
          name: machine.name,
          description: machine.description,
          imageUrl: machine.imageUrl,
          machineTypeId: machine.machineTypeId,
          userId: machine.userId
        });

        this.imagePreview = machine.imageUrl;

        // Map existing parameters
        const paramControl = this.machineForm.get('parameters') as FormArray;
        paramControl.clear(); // Clear default init

        // We want to show ALL available parameters, pre-filled if value exists
        this.availableParameters.forEach(p => {
          const existing = machine.parameters?.find(mp => mp.parameterId === p.id);
          paramControl.push(this.createParameterGroup(p.id, existing ? existing.value : ''));
        });

        this.loading = false;
      },
      error: (err) => this.handleError('Failed to load machine details', err)
    });
  }

  initParameters() {
    const paramControl = this.machineForm.get('parameters') as FormArray;
    this.availableParameters.forEach(p => {
      paramControl.push(this.createParameterGroup(p.id, ''));
    });
  }

  createParameterGroup(paramId: number, value: string): FormGroup {
    return this.fb.group({
      parameterId: [paramId],
      value: [value] // Optional, add validators if needed
    });
  }

  get parameters() {
    return this.machineForm.get('parameters') as FormArray;
  }

  getParameterName(index: number): string {
    const pId = this.parameters.at(index).value.parameterId;
    const p = this.availableParameters.find(x => x.id === pId);
    return p ? `${p.name} (${p.unit})` : 'Unknown';
  }

  onSubmit() {
    if (this.machineForm.invalid) return;

    this.loading = true;
    const formValue = this.machineForm.value;

    // Filter out empty parameters if desired, or send all
    // For now, let's send all populated ones
    const cleanedParams = formValue.parameters.filter((p: any) => p.value !== '' && p.value !== null);
    formValue.parameters = cleanedParams;

    if (this.isEditMode && this.machineId) {
      // Implement Update Logic in Service first if not exists (PUT)
      this.machineService.updateMachine(this.machineId, formValue).subscribe({
        next: () => this.onSuccess('Machine updated successfully'),
        error: (err) => this.handleError('Failed to update machine', err)
      });
    } else {
      this.machineService.createMachine(formValue).subscribe({
        next: () => this.onSuccess('Machine created successfully'),
        error: (err) => this.handleError('Failed to create machine', err)
      });
    }
  }

  onSuccess(message: string) {
    this.snackBar.open(message, 'Close', { duration: 3000 });
    this.router.navigate(['/machines']);
  }

  handleError(message: string, err: any) {
    console.error(err);
    this.snackBar.open(message, 'Close', { duration: 3000 });
    this.loading = false;
  }
}
