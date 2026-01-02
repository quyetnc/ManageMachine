import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MachineService } from 'src/app/core/services/machine.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-parameter-form',
  templateUrl: './parameter-form.component.html',
  styleUrls: ['./parameter-form.component.scss']
})
export class ParameterFormComponent implements OnInit {
  paramForm: FormGroup;
  isEditMode = false;
  id: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private machineService: MachineService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.paramForm = this.fb.group({
      name: ['', Validators.required],
      unit: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.id = +id;
      this.loadParameter(this.id);
    }
  }

  loadParameter(id: number) {
    this.machineService.getParameter(id).subscribe({
      next: (data) => this.paramForm.patchValue(data),
      error: (err) => console.error(err)
    });
  }

  onSubmit() {
    if (this.paramForm.invalid) return;
    this.loading = true;

    if (this.isEditMode && this.id) {
      this.machineService.updateParameter(this.id, this.paramForm.value).subscribe({
        next: () => this.onSuccess('Updated successfully'),
        error: (err) => this.onError('Update failed', err)
      });
    } else {
      this.machineService.createParameter(this.paramForm.value).subscribe({
        next: () => this.onSuccess('Created successfully'),
        error: (err) => this.onError('Create failed', err)
      });
    }
  }

  onSuccess(msg: string) {
    this.snackBar.open(msg, 'Close', { duration: 3000 });
    this.router.navigate(['/parameters']);
  }

  onError(msg: string, err: any) {
    console.error(err);
    this.snackBar.open(msg, 'Close', { duration: 3000 });
    this.loading = false;
  }
}
