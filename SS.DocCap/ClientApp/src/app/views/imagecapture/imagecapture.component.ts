import { Component, OnInit, ViewChild, HostListener, Directive, ElementRef, } from '@angular/core';
import { getStyle, hexToRgba } from '@coreui/coreui/dist/js/coreui-utilities';
import { CustomTooltips } from '@coreui/coreui-plugin-chartjs-custom-tooltips';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { Subject, Observable } from 'rxjs';
import { WebcamImage, WebcamInitError, WebcamUtil } from 'ngx-webcam';
import { FormGroup, FormControl, Validators, NgForm } from '@angular/forms';
import { ImageCaptureService } from '../../views/imagecapture/imageCapture.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  templateUrl: 'imagecapture.component.html',
  styleUrls: ["./imagecapture.style.css"],
})
export class ImagecaptureComponent implements OnInit {
  @ViewChild('staticTabs', { static: false, read: TabsetComponent }) staticTabs: TabsetComponent;
  @ViewChild('documentInfoForm') private formDirective: NgForm;
  progress: number;
  infoMessage: any;
  isUploading: boolean = false;
  file: File;
  RSLPeriods: any[] = [];
  selectedRSLPeriod: any;
  chequeIssue: boolean = false;
  documentNo: string = "";
  documentAmount: string="";
  documentRemark: string = "";
  documentType: string = "";
  isAttachement: string = "false";
  IssubmitButton: boolean = false;
  imageUrl: string | ArrayBuffer;
  IsSiteUpdated:boolean=false;




  fileName: string = "No file selected";

  onChange(file: File) {
    if (file) {
      this.fileName = file.name;
      this.file = file;

      const reader = new FileReader();
      reader.readAsDataURL(file);

      reader.onload = event => {
        this.isAttachement = "true";
        this.imageUrl = reader.result;
      };
    }
  }

  onUpload() {
    this.infoMessage = null;
    this.progress = 0;
    this.isUploading = true;

    //this.uploader.upload(this.file).subscribe(message => {
    //  this.isUploading = false;
    //  this.infoMessage = message;
    //});
  }

  selectTab(tabId: number,documentType:string) {
    this.documentType = documentType;
    this.staticTabs.tabs[tabId].disabled = false;
    this.staticTabs.tabs[tabId].active = true;

  }
  public showWebcam = true;
  public allowCameraSwitch = true;
  public multipleWebcamsAvailable = false;
  public deviceId: string;
  public videoOptions: MediaTrackConstraints = {
    width: { ideal: 1024 },
    height: { ideal: 576 }
  };
  public errors: WebcamInitError[] = [];


  constructor(private _ImageCaptureService: ImageCaptureService, private toastr: ToastrService, private _router: Router,) {
    if (sessionStorage.getItem("dcjwt") === null || sessionStorage.getItem("dcjwt") === 'null' || sessionStorage.getItem("bndjwt") === undefined) {
      this._router.navigate(['']);
    }


  }
  // latest snapshot
  public webcamImage: WebcamImage = null;

  // webcam snapshot trigger
  private trigger: Subject<void> = new Subject<void>();
  // switch to next / previous / specific webcam; true/false: forward/backwards, string: deviceId
  private nextWebcam: Subject<boolean | string> = new Subject<boolean | string>();

  public ngOnInit(): void {
if(sessionStorage.getItem("dcbranchcode") ===""){
this.IsSiteUpdated=false;
}else{
  this.IsSiteUpdated=true;
}

    this.getRSLPeriods();
    WebcamUtil.getAvailableVideoInputs()
      .then((mediaDevices: MediaDeviceInfo[]) => {
        this.multipleWebcamsAvailable = mediaDevices && mediaDevices.length > 1;
      });
  }

  public triggerSnapshot(): void {
    this.trigger.next();
  }

  public toggleWebcam(): void {
    this.showWebcam = !this.showWebcam;
  }

  public handleInitError(error: WebcamInitError): void {
    this.errors.push(error);
  }

  public showNextWebcam(directionOrDeviceId: boolean | string): void {
    // true => move forward through devices
    // false => move backwards through devices
    // string => move to device with given deviceId
    this.nextWebcam.next(directionOrDeviceId);
  }

  public handleImage(webcamImage: WebcamImage): void {
    //console.info('received webcam image', webcamImage);
    this.isAttachement = "false";
    this.webcamImage = webcamImage;
    this.imageUrl = this.webcamImage.imageAsDataUrl;
  }

  public cameraWasSwitched(deviceId: string): void {
    //console.log('active device: ' + deviceId);
    this.deviceId = deviceId;
  }

  public get triggerObservable(): Observable<void> {
    return this.trigger.asObservable();
  }

  public get nextWebcamObservable(): Observable<boolean | string> {
    return this.nextWebcam.asObservable();
  }

  ClearForm() {
    this.selectedRSLPeriod = undefined;
    this.chequeIssue = false;
    this.documentNo = "";
    this.documentAmount = "";
    this.documentRemark = "";   
    this.isAttachement = "false";
    this.imageUrl = "";
    //this.formDirective.resetForm();
    this.setCurrentRSLPeriod();

  }

  getRSLPeriods() {
    this._ImageCaptureService.getRSLPeriods().then((data) => {

      //console.log("RSL Periods:", data);

      this.RSLPeriods = data;

      var tempselectedRSLPeriod = this.RSLPeriods.filter(
        rsl => rsl.isCurrent === true);

      if (tempselectedRSLPeriod.length > 0) {
        this.selectedRSLPeriod = tempselectedRSLPeriod[0];
        //console.log("Current RSL:", this.selectedRSLPeriod);
      }


    }).catch((error) => {
     
      this.toastr.error('', 'Netowork Error !, Please check the network connection or reload the page', {
        timeOut: 6000, positionClass: "toast-top-center"
      });
    });
  }

  setCurrentRSLPeriod(){
    var tempselectedRSLPeriod = this.RSLPeriods.filter(
      rsl => rsl.isCurrent === true);

    if (tempselectedRSLPeriod.length > 0) {
      this.selectedRSLPeriod = tempselectedRSLPeriod[0];
      //console.log("Current RSL:", this.selectedRSLPeriod);
    }
  }


  UploadImage(): any {

    //console.log("chequeIssue", this.chequeIssue);
    //console.log("selectedRSLPeriod", this.selectedRSLPeriod);
    //console.log("documentNo", this.documentNo);
    //console.log("documentAmount", this.documentAmount);
    //console.log("documentRemark", this.documentRemark);

    if (this.imageUrl == undefined || this.imageUrl == "") {
      this.toastr.error('', 'Please capture/select a image', {
        timeOut: 6000, positionClass: "toast-top-center"
      });
      return false;
    }
    if (this.selectedRSLPeriod == undefined  || this.selectedRSLPeriod == "") {
      this.toastr.error('','Please select the RSL field', {
        timeOut: 6000, positionClass: "toast-top-center"
      });
      return false;
    }
    if (this.documentNo == undefined || this.documentNo == "") {
      this.toastr.error('', 'Please feed the Document no field', {
        timeOut: 6000, positionClass: "toast-top-center"
      });
      return false;
    }
    if (this.documentAmount == undefined || this.documentAmount == "") {
      this.toastr.error('', 'Please feed the Document amount no field', {
        timeOut: 6000, positionClass: "toast-top-center"
      });
      return false;
    }

    const formData = new FormData();
    formData.append('AttachedImage', this.file);
    formData.append('DocumentType', this.documentType);
    formData.append('chequeIssue', this.chequeIssue.toString());
    formData.append('RSLPeriod', this.selectedRSLPeriod.rslPeriodTitle);
    formData.append('RSLPeriodId', this.selectedRSLPeriod.rslid);
    formData.append('DocumentNo', this.documentNo);
    formData.append('DocumentAmount', this.documentAmount);
    formData.append('Remark', this.documentRemark);
    formData.append('isAttachedment', this.isAttachement);
    if (this.isAttachement == "true") {
      formData.append('AttachedImage', this.file);
    } else {
      formData.append('CapturedImage', this.webcamImage.imageAsBase64);
    }
   
    
    this.IssubmitButton = true;
    this._ImageCaptureService.uploadImage(formData).then((data) => {


     // console.log("Upload Res:", data);
      this.toastr.success('', 'Image successfully uploaded !', {
        timeOut: 6000, positionClass: "toast-top-center"
      });
      this.ClearForm();
      this.IssubmitButton = false;

    }).catch((error) => {
      this.IssubmitButton = false;
      this.toastr.error('', 'Netowork Error !, Please check the network connection or reload the page', {
        timeOut: 6000, positionClass: "toast-top-center"
      });
    });
  }


}



