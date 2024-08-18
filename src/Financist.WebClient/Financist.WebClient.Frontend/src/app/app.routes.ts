import { Routes } from "@angular/router";
import { AuthLayoutComponent } from "@app/layouts/auth-layout/auth-layout.component";
import { MainLayoutComponent } from "@app/layouts/main-layout/main-layout.component";
import { SignUpPageComponent } from "@app/pages/auth/sign-up-page/sign-up-page.component";
import { SignInPageComponent } from "@app/pages/auth/sign-in-page/sign-in-page.component";
import { DashboardPageComponent } from "@app/pages/main/dashboard-page/dashboard-page.component";
import { HomePageComponent } from "@app/pages/home-page/home-page.component";
import { isAuthenticatedGuard } from "@app/guards/is-authenticated.guard";
import { isUnauthenticatedGuard } from "@app/guards/is-unauthenticated.guard";
import { BookPageComponent } from "@app/pages/main/book-page/book-page.component";
import { NewBookPageComponent } from "@app/pages/main/new-book-page/new-book-page.component";

export const routes: Routes = [
  {
    path: "",
    component: HomePageComponent,
    canActivate: [isUnauthenticatedGuard],
  },
  {
    path: "",
    component: AuthLayoutComponent,
    children: [
      {
        path: "sign-up",
        component: SignUpPageComponent,
        canActivate: [isUnauthenticatedGuard],
      },
      {
        path: "sign-in",
        component: SignInPageComponent,
        canActivate: [isUnauthenticatedGuard],
      },
    ],
  },
  {
    path: "",
    component: MainLayoutComponent,
    children: [
      {
        path: "dashboard",
        component: DashboardPageComponent,
        canActivate: [isAuthenticatedGuard],
      },
      {
        path: "book/new",
        component: NewBookPageComponent,
        canActivate: [isAuthenticatedGuard],
      },
      {
        path: "book/:bookId",
        component: BookPageComponent,
        canActivate: [isAuthenticatedGuard],
      },
    ],
  },
];
