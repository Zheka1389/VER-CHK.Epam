import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home';
import { AuthGuard } from './helpers/auth.guard';
import { LoginComponent } from './user-management/forms/login.component';
import { RegisterComponent } from './user-management/forms/register.component';
import { ArticleComponent } from './article-management/lists/article.component';
import { ArticleCreateComponent } from './article-management/lists/articleCreate.component';
import { ArticleFullComponent } from './article-management/lists/articleFull.component';
import { LoginSettingsComponent } from './user-management/forms/loginSettings.component';

const routes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'article', component: ArticleComponent },
    { path: 'articleCreate', component: ArticleCreateComponent },
    { path: 'articleFull', component: ArticleFullComponent },
    { path: 'articleFull/:id', component: ArticleFullComponent },
    { path: 'loginSettings', component: LoginSettingsComponent, canActivate: [AuthGuard] },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const appRoutingModule = RouterModule.forRoot(routes);