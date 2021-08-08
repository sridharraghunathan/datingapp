import { Injectable } from '@angular/core';
import { CanDeactivate  } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(component: MemberEditComponent): boolean {
    console.log('I am in deactive');
    if (component.editForm.dirty) {
      return  confirm('Are you sure you want to continue? Any unsaved changes will be lost');
    }
    return true;
  }

}