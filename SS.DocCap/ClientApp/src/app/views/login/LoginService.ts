import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import {
  ActivatedRouteSnapshot,
  Resolve,
  RouterStateSnapshot,
} from "@angular/router";
import { BehaviorSubject, Observable, Subject } from "rxjs";

@Injectable()
export class LoginService {
  responceData: any[];
  UserLevel: any[];
  ClientSettings: any[];
  _baseUrl: string;
  private onContactsChanged: BehaviorSubject<any> = new BehaviorSubject([]);
  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    console.log(baseUrl);
    this._baseUrl = baseUrl;
    //this.onContactsChanged = new BehaviorSubject([]);
  }


  loginUser(formData): Promise<any> {
    return new Promise((resolve, reject) => {
      this._httpClient
        .post(this._baseUrl + "api/UserAuth", formData)
        .subscribe((response: any) => {
          this.responceData = response;
         // console.log("responceData :", this.responceData)
          resolve(this.responceData);
        }, reject);

    });
  }

  getUserPermisionLevel(email): Promise<any> {
    return new Promise((resolve, reject) => {
      this._httpClient
        .get(this._baseUrl + "api/UserAuth/GetPermissionLevel?email=" + email)
        .subscribe((response: any) => {
          this.UserLevel = response;
          resolve(this.UserLevel);
        }, reject);
    });
  }

  loginMultiUser(formData): Promise<any> {
    return new Promise((resolve, reject) => {


      this._httpClient
        .post(this._baseUrl + "api/UserAuth/multiauth", formData)
        .subscribe((response: any) => {
          // this.contacts = {};
          this.responceData = response;
          //console.log("responceData :", this.responceData)
          resolve(this.responceData);
          //resolve(this.contacts);
          // setTimeout(() => {
          //     resolve(this.contacts);
          // }, 100000);
        }, reject);
    });
  }

  getClientSettings(pkey): Promise<any> {
    return new Promise((resolve, reject) => {
      this._httpClient
        .get(this._baseUrl + "api/ClientSettings?key=" + pkey)
        .subscribe((response: any) => {
          this.ClientSettings = response;
          resolve(this.ClientSettings);
        }, reject);
    });
  }
}
