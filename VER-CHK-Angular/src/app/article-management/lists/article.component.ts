import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { Router } from '@angular/router';
import { Article } from '../models/article';
import { AuthenticationService } from '../../user-management/services/authentication.service';
import { ArticleService } from '../services/article.service';
import { CommentArticle } from '../models/commentArticle';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { User } from '../../user-management/models/user';
@Component({ templateUrl: 'article.component.html' })
export class ArticleComponent implements OnInit {
    currentArticle: Article;
    currentUser: User;
    findForm: FormGroup;
    articles: Article[];
    article: Article;
    tArticle = new CommentArticle('test','test', 'test', 'test');

    constructor(
        private authenticationService: AuthenticationService,
        private articleService: ArticleService,
        private router: Router
    ) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    }

    ngOnInit() {
        this.loadAllUsers();
        this.findForm = new FormGroup({
            "find": new FormControl('', [Validators.required, Validators.minLength(1)])
        });
    }

    findTitleOrUser() {
        console.log(this.findForm.controls['find'].value);
        this.articleService.getTitle(this.findForm.controls['find'].value)
            .subscribe(article => this.articles = article);
    }


    getArticle(title: string) {
            this.router.navigate([`/articleFull/${title}`]);
    }

    addComment() {
        this.articleService.addComment(this.tArticle)
            .subscribe();
    }

    private loadAllUsers() {
        this.articleService.getAll()
            .pipe(first())
            .subscribe(articles => this.articles = articles);
    }
}
