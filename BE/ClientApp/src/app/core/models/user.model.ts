export enum UserRole {
    Admin = 0,
    User = 1
}

export interface User {
    id: number;
    username: string;
    fullName: string;
    email: string;
    role: UserRole;
}

export interface CreateUserDto {
    username: string;
    password?: string;
    fullName: string;
    email: string;
    role: UserRole;
}

export interface ResetPasswordDto {
    newPassword: string;
}

export interface UpdateUserDto {
    fullName: string;
    email: string;
    role: UserRole;
}
