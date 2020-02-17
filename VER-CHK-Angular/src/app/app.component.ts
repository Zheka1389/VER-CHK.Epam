import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from './user-management/models/user';
import { AuthenticationService } from './user-management/services/authentication.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ArticleService } from './article-management/services/article.service';
import { Article } from './article-management/models/article';

@Component({ selector: 'app', templateUrl: 'app.component.html' })
export class AppComponent {
    currentUser: User;
    findForm: FormGroup;
    articles: Article[];
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService,
        private articleService: ArticleService
    ) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    }

    ngOnInit() {
        this.findForm = new FormGroup({
            "find": new FormControl('', [Validators.required, Validators.minLength(1)])
        });
    }

    findTitleOrUser() {
        console.log(this.findForm.controls['find'].value);    
        this.articleService.getTitle(this.findForm.controls['find'].value)
            .subscribe(article => this.articles = article);
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}