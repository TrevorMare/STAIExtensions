import { Injectable, OnDestroy } from '@angular/core';
import * as uuid from 'uuid';
import { environment } from '../../environments/environment'
import { Subject, BehaviorSubject } from 'rxjs';
import { DataSetInformation } from '../data/dataset-information'
import { ViewInformation } from '../data/view-information';
import { View } from '../data/view';

declare var STAIExtensionsHub: any;

@Injectable({
  providedIn: 'root'
})
export class STAIExtensionsService implements OnDestroy  {
  private dateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.{0,1}\d*Z$/;
  private ownerId: string = uuid.v4();
  private managedClientHub: any;
  private ownedViewIds: string[] = new Array();
  
  public Initializing$ = new BehaviorSubject<boolean>(false);
  public Ready$ = new BehaviorSubject<boolean>(false);
  public DataContractDataSet$ = new BehaviorSubject<DataSetInformation>(null!);
  public RegisteredViewTypes$ = new BehaviorSubject<ViewInformation[]>(null!);
  public ViewUpdated$ = new Subject<View>();
  public DataSetUpdated$ = new Subject<string>();

  constructor(
  ) { 
    // Workaround for zone.js that overrides the Web Socket open number to a method
    Object.defineProperty(WebSocket, 'OPEN', { value: 1, });

    // Setup the managed client hub
    this.managedClientHub = new STAIExtensionsHub(
      this.ownerId,
      environment.signalRHost,
      environment.signalRAuthKey,
      (dsId: string) => this.OnDataSetUpdated(dsId), 
      (viewId: string) => this.OnDataSetViewUpdated(viewId)
    );

    // Set up the default data set and views
    this.Initializing$.subscribe(isInitializing => {
      if (isInitializing === true) {
        this.SetupServiceDataSet().then((dataSet) => {
          this.DataContractDataSet$.next(dataSet);
          this.SetupHostViews().then((views) => {
            this.RegisteredViewTypes$.next(views);
            this.Ready$.next(true);
          }).catch((err) => {
            console.log(err);
          });
        }).catch((err) => {
          console.log(err);
        });
      }
    })

    // Wait for the connection to establish and initialise the service
    setTimeout(() => {
      
      this.Initializing$.next(true);
    }, 1000);

  }
 
  ngOnDestroy() {
    this.Ready$.next(false);
  }

  public async CreateView$(viewTypeName: string): Promise<View> {

    return new Promise((resolve, reject) => {

      if (this.Ready$.value === false) throw `Service not ready`;
      // Create the view
      this.managedClientHub.CreateView(viewTypeName, (_: any, view: View) => {
        // Attach the view to the data set
        this.managedClientHub.AttachViewToDataset(view.id, this.DataContractDataSet$.value.dataSetId, (_: any, success: boolean) => {
          if (success === true) {

            this.ownedViewIds.push(view.id);
            this.reviveObject(view);
            resolve(view);
          } else {
            reject('Could not create view');
          }}, (err: any) => {
              reject(err);
          });          
      }, (err: any) => {
        reject(err);
      })
    });
  }

  public async GetView$(viewId: string): Promise<View> {
    return new Promise((resolve, reject) => {

      if (this.Ready$.value === false) throw `Service not ready`;
      // Create the view
      this.managedClientHub.GetView(viewId, (_: any, view: View) => {
        this.reviveObject(view);
        resolve(view);
      }, (err: any) => {
        reject(err);
      })
    });
  }

  public async LoadView$(viewId: string): Promise<any> {
    return new Promise((resolve, reject) => {
      this.managedClientHub.GetView(viewId, (_: any, view: any) => {
        this.reviveObject(view);
        resolve(view);
      }, (err: any) => {
          reject(err);
      });
    });
  }

  public async RemoveView$(viewId: string): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this.managedClientHub.RemoveView(viewId, (_: any, result: boolean) => {
        var ownedIx = this.ownedViewIds.indexOf(viewId);
        if (ownedIx >= 0) {
          this.ownedViewIds.splice(ownedIx);
        }
        resolve(result);
      }, (err: any) => {
          reject(err);
      });
    });
  }

  public async SetViewParameters(viewId: string, parameters: any): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this.managedClientHub.SetViewParameters(viewId, parameters, (_: any, result: boolean) => {
        resolve(result);
      }, (err: any) => {
          reject(err);
      });
    });
  }

  private OnDataSetUpdated(dataSetId: string): void {
    this.DataSetUpdated$.next(dataSetId);
  }

  private OnDataSetViewUpdated(viewId: string): void {
    
    if (this.ownedViewIds.indexOf(viewId) >= 0) {
      this.LoadView$(viewId).then((view) => {
        this.ViewUpdated$.next(view);
      })
    }
  }

  private async SetupServiceDataSet(): Promise<DataSetInformation> {
    return new Promise((resolve, reject) => { 
      this.managedClientHub.ListDataSets((_ : any, dataSets: DataSetInformation[]) => {
        if (!!dataSets && dataSets.length) {
          var dataSet = dataSets.find(x => x.dataSetType.indexOf('DataContractDataSet') >= 0);
          if (!!dataSet) {
            this.reviveObject(dataSet);
            resolve(dataSet);
          } else {
            reject(`Default data set could not be found!`);
          }
        }
      }, (err: any) => {
        reject(err);
      });
    });
  }

  private async SetupHostViews(): Promise<ViewInformation[]> {
    return new Promise((resolve, reject) => {

      this.managedClientHub.GetRegisteredViews((_ : any, views: ViewInformation[]) => {
        if (!!views && views.length) {
          this.reviveObject(views);
          resolve(views);
        } else {
          reject('No views registered on the server.');
        }
      }, (err: any) => {
        reject(err);
      });
    });
  }

  

  private reviveObject(obj: any) {
    if (obj === undefined || obj === null) return;
    for (var property in obj) {
        if (obj.hasOwnProperty(property)) {
            if (typeof obj[property] == "object") {
                this.reviveObject(obj[property]);
            }
            else {
               obj[property] = this.dateReviver(obj[property]);
            }
        }
    }
  }

  private dateReviver(value) {
    if (typeof value === "string" && this.dateFormat.test(value)) {
      return new Date(value);
    }
    return value;
  }

}
