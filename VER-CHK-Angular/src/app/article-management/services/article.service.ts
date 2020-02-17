import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article } from '../models/article';
import { CommentArticle } from '../models/commentArticle';

@Injectable({ providedIn: 'root' })
export class ArticleService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<Article[]>(`${config.apiUrl}/articles`);
    }

    get(title: string) {
        return this.http.get<Article>(`${config.apiUrl}/articles/${title}`);
    }

    getTitle(title: string) {
        return this.http.get<Article[]>(`${config.apiUrl}/articles/search/${title}`);
    }

    create(article: Article): Observable<Article> {
        return this.http.post<Article>(`${config.apiUrl}/articles/create`, article);
    }

    addComment(article: CommentArticle) {
        return this.http.post(`${config.apiUrl}/articles/title`, article);
    }

    update(article: Article) {
        return this.http.put(`${config.apiUrl}/articles/update`, article);
    }

    delete(articleName: string) {
        return this.http.delete(`${config.apiUrl}/articles/${articleName}`);
    }
}