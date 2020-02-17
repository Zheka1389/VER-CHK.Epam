import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Article } from '../models/article';
import { AuthenticationService } from '../../user-management/services/authentication.service';
import { ArticleService } from '../services/article.service';
import { AlertService } from '../../helpers/alert.service';
import { User } from '../../user-management/models/user';


@Component({ templateUrl: 'articleCreate.component.html' })
export class ArticleCreateComponent implements OnInit {
    currentArticle = new Article('', '', '', '', 'test', '', '');
    currentUser: User;
    articles = [];
    existed = false;
    createForm: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private authenticationService: AuthenticationService,
        private articleService: ArticleService,
        private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private alertService: AlertService
    ) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    }
    ngOnInit() {
        this.createForm = this.formBuilder.group({
            title: ['', Validators.required],
            category: ['', Validators.required],
            content: ['', [Validators.required, Validators.maxLength(2000)]]
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.createForm.controls; }
    onSubmit() {
        console.log(this.createForm.value);
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.createForm.invalid) {
            return;
        }

        this.loading = true;

        this.currentArticle.title = this.createForm.controls['title'].value;
        this.currentArticle.category = this.createForm.controls['category'].value;
        this.currentArticle.content = this.createForm.controls['content'].value;
        this.currentArticle.createdUser = this.authenticationService.currentUserValue.userName;
        this.articleService.create(this.currentArticle)
            .subscribe(
                data => {
                    
                    this.alertService.success('Create successful', true);
                    this.router.navigate(['/article']);
                },
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });
    }
}
