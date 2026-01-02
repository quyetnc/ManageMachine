import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MachineService } from 'src/app/core/services/machine.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-type-form',
  templateUrl: './type-form.component.html',
  styleUrls: ['./type-form.component.scss']
})
export class TypeFormComponent implements OnInit {
  typeForm: FormGroup;
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
    this.typeForm = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.id = +id;
      this.loadType(this.id);
    }
  }

  loadType(id: number) {
    this.machineService.getMachineType(id).subscribe({
      next: (data) => this.typeForm.patchValue(data),
      error: (err) => console.error(err)
    });
  }

  onSubmit() {
    if (this.typeForm.invalid) return;
    this.loading = true;

    if (this.isEditMode && this.id) {
      this.machineService.updateMachineType(this.id, this.typeForm.value).subscribe({
        next: () => this.onSuccess('Updated successfully'),
        error: (err) => this.onError('Update failed', err)
      });
    } else {
      this.machineService.createMachineType(this.typeForm.value).subscribe({
        next: () => this.onSuccess('Created successfully'),
        error: (err) => this.onError('Create failed', err)
      });
    }
  }

  onSuccess(msg: string) {
    this.snackBar.open(msg, 'Close', { duration: 3000 });
    this.router.navigate(['/machine-types']);
  }

  onError(msg: string, err: any) {
    console.error(err);
    this.snackBar.open(msg, 'Close', { duration: 3000 });
    this.loading = false;
  }
}
