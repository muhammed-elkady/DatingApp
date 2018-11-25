import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  success(msg: string) {
    alertify.success(msg);
  }

  error(msg: string) {
    alertify.error(msg);
  }
  warning(msg: string) {
    alertify.warning(msg);
  }
  message(msg: string) {
    alertify.message(msg);
  }

  confirm(msg: string, okCallback: () => any) {
    alertify.confirm(msg, (clickEvent) => {
      if (clickEvent) {
        okCallback();
      }
      else { }
    });
  }



}
