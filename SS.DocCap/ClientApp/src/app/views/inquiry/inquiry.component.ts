import { Component, OnInit, ViewChild, HostListener, Directive, ElementRef, } from '@angular/core';
import { getStyle, hexToRgba } from '@coreui/coreui/dist/js/coreui-utilities';
import { CustomTooltips } from '@coreui/coreui-plugin-chartjs-custom-tooltips';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { Subject, Observable } from 'rxjs';
import { WebcamImage, WebcamInitError, WebcamUtil } from 'ngx-webcam';
import { FormGroup, FormControl, Validators, NgForm } from '@angular/forms';
import { InquiryService } from '../inquiry/inquiry.service';
import { ToastrService } from 'ngx-toastr';
import { Location, DatePipe, Time } from '@angular/common';
import { DataTableDirective } from 'angular-datatables';
import { Router } from '@angular/router';
import { ImageCaptureService } from '../../views/imagecapture/imageCapture.service';

@Component({
  templateUrl: 'inquiry.component.html',
  styleUrls: ["./inquiry.style.css"],
})
export class InquiryComponent implements OnInit {
  @ViewChild('documentInfoForm') private formDirective: NgForm;
  @ViewChild(DataTableDirective, { static: false })
  dtElement: DataTableDirective;
  isDtInitialized: boolean = false;
  progress: number;
  infoMessage: any;
  isUploading: boolean = false;
  file: File;
  RSLPeriods: any[] = [];
  selectedRSLPeriod: any = "";
  chequeIssue: boolean = false;
  documentNo: string = "";
  documentAmount: string = "";
  documentRemark: string = "";
  documentType: string = "";
  isAttachement: string = "false";
  IssubmitButton: boolean = false;
  imageUrl: string | ArrayBuffer;
  selectedDocType: string = "";
  fromDate: string = "";
  toDate: string = "";
  searchresult: any[];
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  schequeIssue: boolean = false;
  sdocumentNo: string = "";
  sdocumentAmount: string = "";
  sdocumentRemark: string = "";
  sdocumentType: string = "";
  sselectedRSLPeriod: any = "";

  constructor(private _InquiryService: InquiryService, private _ImageCaptureService: ImageCaptureService, private toastr: ToastrService, public datepipe: DatePipe, private _router: Router,) {
    if (sessionStorage.getItem("dcjwt") === null || sessionStorage.getItem("dcjwt") === 'null' || sessionStorage.getItem("dcjwt") === undefined) {
      this._router.navigate(['']);
    }
  }

  public ngOnInit(): void {
   // this.search();
    this._ImageCaptureService.getRSLPeriods().then((data) => {
      this.RSLPeriods = data;
    }).catch((error) => {
      this.toastr.error('', 'Netowork Error !, Please check the network connection or reload the page', {
        timeOut: 6000, positionClass: "toast-top-center"
      });
    });
  }

  ClearForm() {
    this.selectedRSLPeriod = undefined;
    this.chequeIssue = false;
    this.documentNo = "";
    this.documentAmount = "";
    this.documentRemark = "";
    this.isAttachement = "false";
    this.imageUrl = "";
    this.schequeIssue = false;
    this.sdocumentNo = "";
    this.sdocumentAmount = "";
    this.sdocumentRemark = "";
    this.sdocumentType = "";
    this.sselectedRSLPeriod = "";
    this.fromDate = "";
    this.toDate = "";
    this.selectedDocType = "";
    this.selectedRSLPeriod = [];
    this.documentNo = "";
    this.documentAmount = ""
    this.chequeIssue = false;
    this.ngOnInit();
    //this.formDirective.resetForm();
  }
  selectdata(selectedData) {
    this.schequeIssue = selectedData.isChequeIssue;
    this.sdocumentNo = selectedData.documentNo;
    this.sdocumentAmount = selectedData.documentAmount;
    this.sdocumentRemark = selectedData.remark;
    this.sdocumentType = selectedData.documentType;
    this.sselectedRSLPeriod = selectedData.rslPeriod;
    this.imageUrl = selectedData.rootUrl + selectedData.documentUrl;
  }

  search(): any {

    this.imageUrl = "";
    this.schequeIssue = false;
    this.sdocumentNo = "";
    this.sdocumentAmount = "";
    this.sdocumentRemark = "";
    this.sdocumentType = "";
    this.sselectedRSLPeriod = "";

    var searchRSL = "";
    var searchRSLId = "";
    //console.log("this.selectedRSLPeriod", this.selectedRSLPeriod);
    if (this.selectedRSLPeriod == undefined || this.selectedRSLPeriod == "") {
      searchRSL = "";
    } else {
      searchRSL = this.selectedRSLPeriod.rslPeriodTitle;
      searchRSLId = "";
    }

    var search = {
      fromDate: this.fromDate,
      toDate: this.toDate,
      documentType: this.selectedDocType,
      rslPeriod: searchRSL,
      rSLPeriodId:"",
      documentNo: this.documentNo,
      documentAmount: this.documentAmount,
      isChequeIssue: this.chequeIssue
    }

    this._InquiryService.search(search).then((data) => {
      //console.log("search", data);
      this.searchresult = data
      //this.dtTrigger.next();
      //this.dtOptions = {
      //  pagingType: 'full_numbers',
      //  pageLength: 10
      //};
      if (this.isDtInitialized) {
        this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
          dtInstance.destroy();
          this.dtTrigger.next();
        });
      } else {
        this.isDtInitialized = true
        this.dtTrigger.next();
      }
    }).catch((error) => {
      this.searchresult = [];
    });

  }
}



