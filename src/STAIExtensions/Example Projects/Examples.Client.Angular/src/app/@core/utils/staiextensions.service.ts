import { Injectable } from '@angular/core';
import * as uuid from 'uuid';
import { environment } from '../../../environments/environment'
import { of, Subject, interval, BehaviorSubject, pipe } from 'rxjs';
import { DataSetInformation } from '../data/dataset-information'
import { ViewInformation } from '../data/view-information';
import { View } from '../data/view';
import { switchMap, first } from 'rxjs/operators';
import { filter } from 'rxjs-compat/operator/filter';

declare var STAIExtensionsHub: any;

@Injectable({
  providedIn: 'root'
})
export class STAIExtensionsService {
  
  private ownerId: string = uuid.v4();
  private managedClientHub: any;
  private ownedViewIds: string[] = new Array();
  
  public Initializing$ = new BehaviorSubject<boolean>(false);
  public Ready$ = new BehaviorSubject<boolean>(false);
  public DataSet$ = new BehaviorSubject<DataSetInformation>(null);
  public HostViews$ = new BehaviorSubject<ViewInformation[]>(null);
  public ViewUpdated$ = new Subject<View>();

  constructor(
  ) { 
    // Workaround for zone.js that overrides the Web Socket open number to a method
    Object.defineProperty(WebSocket, 'OPEN', { value: 1, });

    this.managedClientHub = new STAIExtensionsHub(
      this.ownerId,
      environment.signalRHost,
      environment.signalRAuthKey,
      this.OnDataSetUpdated, this.OnDataSetViewUpdated
    );

    // Set up the default data set and views
    this.Initializing$.subscribe(isInitializing => {
      if (isInitializing === true) {
        this.SetupServiceDataSet().then((dataSet) => {
          this.DataSet$.next(dataSet);
          this.SetupHostViews().then((views) => {
            this.HostViews$.next(views);
            this.Ready$.next(true);
          }).catch((err) => {
            console.log(err);
          });
        }).catch((err) => {
          console.log(err);
        });
      }
    })

    setTimeout(() => {
      this.Initializing$.next(true);
    }, 500);

  }
 
  public async CreateView(viewTypeName: string): Promise<View> {

    return new Promise((resolve, reject) => {

      if (this.Ready$.value === false) throw `Service not ready`;

      this.managedClientHub.CreateView(viewTypeName, (_: any, view: View) => {

        console.log(view);
        console.log(this.DataSet$.value.dataSetId);

        this.managedClientHub.AttachViewToDataset(view.id, this.DataSet$.value.dataSetId, (_: any, success: boolean) => {
          if (success === true) {
            this.ownedViewIds.push(view.id);
            resolve(view);
          } else {
            reject('Could not create view');
          }             
          }, (err: any) => {
              reject(err);
          });          
      }, (err: any) => {
        console.log(err);
        reject(err);
      })
    });
  }

  public async LoadView(viewId: string): Promise<View> {
    console.log(`Reading view from host`);
    return new Promise((resolve, reject) => {
      this.managedClientHub.GetView(viewId, (_: any, view: View) => {
        resolve(view);
      }, (err) => {
          reject(err);
      });
    });
  }

  protected OnDataSetUpdated(dataSetId: string): void {}

  protected OnDataSetViewUpdated(viewId: string): void {
    if (this.ownedViewIds.indexOf(viewId) > 0) {
      this.LoadView(viewId).then((view) => {
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
          resolve(views);
        } else {
          reject('No views registered on the server.');
        }
      }, (err: any) => {
        reject(err);
      });
    });
  }

}
