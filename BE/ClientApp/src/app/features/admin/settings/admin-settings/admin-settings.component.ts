import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-admin-settings',
    templateUrl: './admin-settings.component.html',
    styleUrls: ['./admin-settings.component.scss']
})
export class AdminSettingsComponent {
    selectedFile: File | null = null;
    previewUrl: string | ArrayBuffer | null = null;
    uploading = false;

    constructor(private http: HttpClient, private snackBar: MatSnackBar) { }

    onFileSelected(event: any) {
        const file = event.target.files[0];
        if (file) {
            this.selectedFile = file;

            // Preview
            const reader = new FileReader();
            reader.onload = e => this.previewUrl = reader.result;
            reader.readAsDataURL(file);
        }
    }

    uploadBackground() {
        if (!this.selectedFile) return;

        this.uploading = true;
        const formData = new FormData();
        formData.append('file', this.selectedFile);

        this.http.post(`${environment.apiUrl}/Settings/login-background`, formData)
            .subscribe({
                next: () => {
                    this.uploading = false;
                    this.snackBar.open('Background updated successfully!', 'Close', { duration: 3000 });
                    this.selectedFile = null;
                    // Keep preview for visual confirmation or reset? Let's keep it.
                },
                error: (err) => {
                    this.uploading = false;
                    this.snackBar.open('Failed to upload background', 'Close', { duration: 3000 });
                    console.error(err);
                }
            });
    }
}
