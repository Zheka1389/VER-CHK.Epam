import { Component, OnInit, SimpleChanges, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Article } from '../models/article';
import { CommentArticle } from '../models/commentArticle';
import { AuthenticationService } from '../../user-management/services/authentication.service';
import { ArticleService } from '../services/article.service';
import { AlertService } from '../../helpers/alert.service';
import { User } from '../../user-management/models/user';


@Component({ templateUrl: 'articleFull.component.html' })
export class ArticleFullComponent implements OnInit {
    article: Article;
    currentArticle: Article;
    currentUser: User;
    articles = [];
    addCommentForm: FormGroup;
    submitted = false;
    te: string;
    tArticle = new CommentArticle('test','test','test','test');
    constructor(
        private authenticationService: AuthenticationService,
        private articleService: ArticleService,
        private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private alertService: AlertService
    ) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
        this.addCommentForm = null;
        this.route.params.subscribe(p => {
            this.articleService.get(p['id']).subscribe(h => { this.article = h, this.te = h.title });
        });
    }

    ngOnInit() {
        this.addCommentForm = new FormGroup({
            "comment": new FormControl('', [Validators.required, Validators.maxLength(200)])
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.addCommentForm.controls; }

    onSubmit() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.addCommentForm.invalid) {
            return;
        }

        this.tArticle.title = this.te;
        this.tArticle.createdUser = this.authenticationService.currentUserValue.userName;
        this.tArticle.comment = this.addCommentForm.controls['comment'].value;
        console.log(this.tArticle);
        this.articleService.addComment(this.tArticle)
            .subscribe(data => {
                this.addCommentForm = new FormGroup({
                    "comment": new FormControl('')
                });
                this.alertService.success('Comment Add', true);
                this.toNavigate(this.tArticle.title);
            },
                error => {
                    this.alertService.error(error);
                });
    }

    toNavigate(title: string) {
        this.route.params.subscribe(p => {
            this.articleService.get(title).subscribe(h =>  this.article = h);
        });
        this.router.navigate([`/articleFull/${title}`]);
    }

    getArticle(title: string) {
        this.articleService.get(title).subscribe(article => this.article = article);
    }
}
