import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UserSessionModel } from "@app/models/user-session.model";

@Injectable({
  providedIn: "root",
})
export class AuthenticationService {
  private httpClient = inject(HttpClient);

  register(email: string, password: string): Observable<object> {
    return this.httpClient.post("api/sign-up", { email, password });
  }

  authenticate(email: string, password: string): Observable<object> {
    return this.httpClient.post("api/sign-in", { email, password });
  }

  logOut(): Observable<object> {
    return this.httpClient.post("api/sign-out", {});
  }

  getUserSession(): Observable<UserSessionModel> {
    return this.httpClient.get<UserSessionModel>("api/session");
  }
}
