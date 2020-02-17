import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';
import { User } from '../user-management/models/user';
import { AuthenticationService } from '../user-management/services/authentication.service';
import { UserService } from '../user-management/services/user.service';

@Component({ templateUrl: 'home.component.html' })
export class HomeComponent implements OnInit {
    users = [];

    constructor(
        private userService: UserService
    ) {}
    

    ngOnInit() {
        this.loadAllUsers();
    }
    private loadAllUsers() {
        this.userService.getAll()
            .pipe(first())
            .subscribe(users => this.users = users);
    }
}