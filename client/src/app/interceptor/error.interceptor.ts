import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router, NavigationExtras } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error) => {
        if (error) {
          switch (error.status) {
            case 400:
              let modalStateErrors = [];
              // For validation Errors Like Form data incorrect
              if (error.error.errors) {
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modalStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modalStateErrors.flat();
              } else if (Array.isArray(error.error)) {
                modalStateErrors = error.error.map(
                  (err: any) => err.description
                );
                throw modalStateErrors;
              }
              // For the Bad request which has no message given from API
              else if (typeof error.error === 'object') {
                this.toastr.error("Bad Request has been made", error.status);
              }
              // For the request which has message given by the API
              else {
                this.toastr.error(error.error, error.status);
              }
              break;
            case 401:
              this.toastr.error(error.error ? error.error : "Not Authorized to Perform this action."
               , error.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {
                state: { error: error.error },
              };
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            case 0:
              this.toastr.error(
                'Backend Server is Unavailable , Please Login After sometime', "503"
              );
              break;
            default:
              this.toastr.error('Something went wrong please try later');
              break;
          }
        }
        return throwError(error);
      })
    );
  }
}
