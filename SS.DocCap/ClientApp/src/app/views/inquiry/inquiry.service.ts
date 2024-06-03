import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import {
  ActivatedRouteSnapshot,
  Resolve,
  RouterStateSnapshot,
} from "@angular/router";
import { BehaviorSubject, Observable, Subject } from "rxjs";

@Injectable()
export class InquiryService {
  response: any;
  _baseUrl: string;
  private onContactsChanged: BehaviorSubject<any> = new BehaviorSubject([]);
  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    console.log(baseUrl);
    this._baseUrl = baseUrl;
    //this.onContactsChanged = new BehaviorSubject([]);
  }

  search(searchObj): Promise<any> {
    return new Promise((resolve, reject) => {
      this._httpClient
        .post(this._baseUrl + "api/UploadImage/Inquiry", searchObj)
        .subscribe((response: any) => {
          this.response = response;
         // console.log("response:", this.response)
          resolve(this.response);      
        }, reject);
    });
  }


}
