import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import {
  ActivatedRouteSnapshot,
  Resolve,
  RouterStateSnapshot,
} from "@angular/router";
import { BehaviorSubject, Observable, Subject } from "rxjs";

@Injectable()
export class ImageCaptureService {
  response: any;
  public RSLPeriods: any[];
  _baseUrl: string;
  private onContactsChanged: BehaviorSubject<any> = new BehaviorSubject([]);
  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
   // console.log(baseUrl);
    this._baseUrl = baseUrl;
    //this.onContactsChanged = new BehaviorSubject([]);
  }

  uploadImage(formData): Promise<any> {
    return new Promise((resolve, reject) => {

      this._httpClient
        .post(this._baseUrl + "api/UploadImage", formData)
        .subscribe((response: any) => {
          this.response = response;
         // console.log("response:", this.response)
          resolve(this.response);      
        }, reject);
    });
  }

  getRSLPeriods(): Promise<any> {
    return new Promise((resolve, reject) => {

      this._httpClient
        .get(this._baseUrl + "api/ExAPI/GetRSLPeriods")
        .subscribe((response: any) => {
          this.RSLPeriods = response;
          //console.log("RSLPeriods:", this.RSLPeriods)
          resolve(this.RSLPeriods);
        }, reject);
    });
  }


}
