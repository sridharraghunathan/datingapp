import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from '../services/confirm.service';

@Injectable({
  providedIn: 'root',
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  constructor(private confirmService: ConfirmService) {}
  //subscription is happening at run time 
  canDeactivate(component: MemberEditComponent): boolean | Observable<boolean> {
    if (component.editForm.dirty) {
      return this.confirmService.confirm();
    }
    return true;
  }
}
