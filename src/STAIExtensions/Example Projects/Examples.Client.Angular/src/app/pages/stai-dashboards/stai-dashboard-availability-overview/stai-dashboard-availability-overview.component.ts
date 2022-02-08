import { Component, OnInit } from '@angular/core';
import { NbSortDirection, NbSortRequest, NbTreeGridDataSource, NbTreeGridDataSourceBuilder } from '@nebular/theme';
import { AvailabilityAggregateGroup } from '../../../@core/data/availability-overview';
import { AvailabilityOverviewService } from '../../../@core/utils/availability-overview.service';



@Component({
  selector: 'ngx-stai-dashboard-availability-overview',
  templateUrl: './stai-dashboard-availability-overview.component.html',
  styleUrls: ['./stai-dashboard-availability-overview.component.scss']
})
export class StaiDashboardAvailabilityOverviewComponent implements OnInit {

  
  constructor(
    public availibilityOverviewService: AvailabilityOverviewService) { 
  }
    
  ngOnInit(): void {
  }
  
  
}
