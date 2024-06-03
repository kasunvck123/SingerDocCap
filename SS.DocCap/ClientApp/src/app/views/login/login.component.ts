import { Component, OnInit, Injectable } from '@angular/core';
import { LoginService } from "./LoginService";
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: 'login.component.html'
})
@Injectable()
export class LoginComponent implements OnInit {
  userName: string = "";
  passwordInput: string = "";
  logindata;
  loginFailed: boolean = false;
  loginText: string = "Login";
  constructor(

    private _LoginService: LoginService,
    private _router: Router,

  ) {
  }
  ngOnInit(): void { }


  login(): void {
    this.loginText = "Please wait !";
    //console.log("Form Dat11a::", this.userName);
    this.logindata = {};
    this.logindata.UserName = this.userName;
    this.logindata.Password = this.passwordInput;
   this.logindata.AuthType = "ApacheLDAP";
    //this.logindata.AuthType = "Service";

    //console.log("Form Data::", this.logindata);

    this._LoginService.loginUser(this.logindata).then((data) => {
      sessionStorage.setItem("dcname", data.name);
      sessionStorage.setItem('dcemail', data.email);
      sessionStorage.setItem('dcjwt', data.jwtToken);
        sessionStorage.setItem("dcbranchcode", data.branchCode);
        sessionStorage.setItem("dcbranchName", data.branchName);
      // sessionStorage.setItem('arpermissionlevel', data.permissionLevel); 
      this._router.navigate(['/imagecapture']);
      this.loginText = "Login";
    }).catch((error) => {
     // console.log("Componet Error::", error.error.detail);
      this.loginFailed = true;
      this.loginText = "Login";
      //alert(error.error.detail);
    });


  }

}
