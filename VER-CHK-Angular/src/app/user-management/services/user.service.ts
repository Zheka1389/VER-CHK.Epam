import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';


@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<User[]>(`${config.apiUrl}/users`);
    }

    get(userName: string) {
        return this.http.get<User>(`${config.apiUrl}/users/${userName}`);
    }

    register(user: User) {
        return this.http.post<User>(`${config.apiUrl}/users/register`, user);
    }

    delete(userName: string) {
        return this.http.delete(`${config.apiUrl}/users/${userName}`);
    }

    update(userName: string, user: User) {
        return this.http.put(`${config.apiUrl}/users/${userName}`, user);
    }
}