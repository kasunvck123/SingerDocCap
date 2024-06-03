import {Component} from '@angular/core';
import { navItems } from '../../_nav';
import { Router } from '@angular/router';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html'
})
export class DefaultLayoutComponent {
  public sidebarMinimized = false;
  public navItems = navItems;
  public userName = "";
  public branchCodeName = "";
  today = new Date();
  todaysDataTime = '';
  constructor(
    private _router: Router,

  ) {
    this.todaysDataTime = formatDate(this.today, 'dd-MM-yyyy', 'en-US');
    this.userName = sessionStorage.getItem("dcname");
    if (sessionStorage.getItem("dcbranchcode") != "" && sessionStorage.getItem("dcbranchName") != "") {
      this.branchCodeName = "| "+sessionStorage.getItem("dcbranchcode") + "-" + sessionStorage.getItem("dcbranchName");
    }
    
  }
  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }

  Logout() {
    sessionStorage.setItem("dcname", "");
    sessionStorage.setItem('dcemail', "");
    sessionStorage.setItem('dcjwt', "");
    sessionStorage.setItem("dcbranchcode", "");
    sessionStorage.setItem("dcbranchName","");
    this._router.navigate(['/']);
  }
}
