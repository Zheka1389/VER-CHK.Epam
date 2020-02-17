import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from '../services/authentication.service';
import { User } from '../models/user';
import { UserService } from '../services/user.service';

import { Router } from '@angular/router';
import { AlertService } from '../../helpers/alert.service';

@Component({ templateUrl: 'loginSettings.component.html' })
export class LoginSettingsComponent implements OnInit {
    currentUser: User;
    users = [];
    updateForm: FormGroup;
    submitted = false;
    user: User;

    constructor(
        private formBuilder: FormBuilder,
        private authenticationService: AuthenticationService,
        private userService: UserService,
        private router: Router,
        private alertService: AlertService
    ) {
        this.currentUser = this.authenticationService.currentUserValue;
    }

    ngOnInit() {
        this.loadUser();
        this.updateForm = this.formBuilder.group({
            email: [this.currentUser.email, Validators.required],
            password: ['', [Validators.required, Validators.minLength(6)]]
        });
        
    }

    deleteUser(userName: string) {
        this.userService.delete(this.currentUser.userName)
            .subscribe();
        this.logout();
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
    onSubmit() {
        this.submitted = true;

        this.alertService.clear();

        if (this.updateForm.invalid) {
            return;
        }
    }

    get f() { return this.updateForm.controls; }

    updateUser(userName: string, user: User) {
        this.userService.update(userName, this.updateForm.value).subscribe(
            data => {
                this.router.navigate(['/']);
        });
    }

    private loadUser() {
        this.userService.get(this.currentUser.userName)
            .subscribe(user => this.user = user);
    }
}