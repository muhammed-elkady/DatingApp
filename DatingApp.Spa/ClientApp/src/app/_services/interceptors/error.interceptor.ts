import { Injectable } from '@angular/core';
import {
    HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(
                catchError(error => {
                    // to handle unauthorized error
                    if (error instanceof HttpErrorResponse) {
                        // debugger;
                        if (error.status === 401) {
                            return throwError("Unauthorized");
                        }
                        const applicationError = error.headers.get('Application-Error');
                        // to handle message error
                        if (applicationError) {
                            console.error(applicationError);
                            return throwError(applicationError);
                        }
                        // to handle array of errors
                        const serverError = error.error;
                        let modalStateErrors = '';
                        if (serverError && typeof serverError === 'object') {
                            for (const key in serverError) {
                                if (serverError[key]) {
                                    modalStateErrors += serverError[key].code + ': ' + serverError[key].description + '\n';
                                }
                            }
                        }
                        return throwError(modalStateErrors || serverError || 'Server Error');
                    }
                })
            );
    }
}


export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
}