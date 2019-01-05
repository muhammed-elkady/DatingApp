import { AbstractControl } from '@angular/forms';

export function matchPassword(form: AbstractControl) {
   return form.get('password').value === form.get('confirmPassword').value ? null : {'missmatch': true}
}