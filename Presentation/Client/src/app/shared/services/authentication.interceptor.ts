import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { AuthenticationService } from './authentication.service';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

    constructor (private authService: AuthenticationService) { }

    intercept(
        req: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        const token = this.authService.getToken();
        
        if (token) {
            req = req.clone({
                setHeaders: { Authorization: `Bearer ${token}` },
                withCredentials: true
            });
        }

        return next.handle(req).pipe(
            catchError((error: HttpErrorResponse) => {
              if (error.status === 401) {
                return this.authService.refreshToken().pipe(
                  switchMap((newToken: string) => {
                    this.authService.setToken(newToken);
                    const clonedReq = req.clone({
                      setHeaders: { Authorization: `Bearer ${newToken}` }
                    });
                    return next.handle(clonedReq);
                  }),
                  catchError((err) => {
                    this.authService.clearToken();
                    return throwError(err);
                  })
                );
              }
              return throwError(error);
            })
        );
    }
}