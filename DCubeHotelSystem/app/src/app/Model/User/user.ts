﻿export interface IUser {
    UserId: number,
    FullName: string,
    UserName: string
    Password: string,
    Email: string,
    PhoneNumber: string,
    IsActive: boolean,
    ResetPassword: boolean
}