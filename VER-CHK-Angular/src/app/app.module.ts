import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { appRoutingModule } from './app.routing';
import { AppComponent } from './app.component';
import { HomeComponent} from './home';
import { AlertComponent } from './_components';
import { LoginComponent } from './user-management/forms/login.component';
import { RegisterComponent } from './user-management/forms/register.component';
import { JwtInterceptor, ErrorInterceptor } from './helpers';
import { ArticleComponent } from './article-management/lists/article.component';
import { ArticleFullComponent } from './article-management/lists/articleFull.component';
import { ArticleCreateComponent } from './article-management/lists/articleCreate.component';
import { LoginSettingsComponent } from './user-management/forms/loginSettings.component';

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpClientModule,
        appRoutingModule
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        LoginSettingsComponent,
        LoginComponent,
        RegisterComponent,
        AlertComponent,
        ArticleComponent,
        ArticleFullComponent,
        ArticleCreateComponent
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { };