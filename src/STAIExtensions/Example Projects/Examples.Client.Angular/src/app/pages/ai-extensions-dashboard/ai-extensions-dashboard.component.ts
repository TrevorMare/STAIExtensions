import { Component, OnInit } from '@angular/core';
import { TelemetryOverviewView } from '../../@core/data/telemetry-overview';
import { View } from '../../@core/data/view';
import { STAIExtensionsService } from '../../@core/utils';

@Component({
  selector: 'ngx-ai-extensions-dashboard',
  templateUrl: './ai-extensions-dashboard.component.html',
  styleUrls: ['./ai-extensions-dashboard.component.scss']
})
export class AiExtensionsDashboardComponent implements OnInit {

  public telemetryOverviewView?: TelemetryOverviewView = null;
  private extensionsService?: STAIExtensionsService = null;

  constructor(staiextensionsService: STAIExtensionsService) { 
    this.extensionsService = staiextensionsService;

    this.extensionsService.Ready$.subscribe((b) => {
      if (b === true) {
        this.CreateViews();
      }
    });
    
    this.extensionsService.ViewUpdated$.subscribe((view) => {
      this.UpdateViews(view);
    });
  }

  ngOnInit(): void {
  }


  private UpdateViews(view: View) {
    this.telemetryOverviewView = {...view};
  }

  private CreateViews(): void {
    this.extensionsService.CreateView("STAIExtensions.Default.Views.TelemetryOverview, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
    .then((view) => {
      this.telemetryOverviewView = view;
    }).catch((err) => {
      console.log(`An error occured ${err}`);
    });
  }

}
