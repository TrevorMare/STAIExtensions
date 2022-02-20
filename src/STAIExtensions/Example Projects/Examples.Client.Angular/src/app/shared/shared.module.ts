import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationMenuComponent } from './navigation-menu/navigation-menu.component';
import { NavigationMenuItemComponent } from './navigation-menu/navigation-menu-item/navigation-menu-item.component';
import { IconComponent } from './icon/icon.component';
import { RouterLink, RouterModule } from '@angular/router';
import { AugmentedCardComponent } from './augmented-card/augmented-card.component';
import { AnimatedHeaderComponent } from './animated-header/animated-header.component';
import { LoadingPanelComponent } from './loading-panel/loading-panel.component';
import { GlitchTextComponent } from './glitch-text/glitch-text.component';
import { JsonObjectViewerComponent } from './json-object-viewer/json-object-viewer.component';
import { PopupModalComponent } from './popup-modal/popup-modal.component';
import { JsonObjectViewerModalComponent } from './json-object-viewer/json-object-viewer-modal/json-object-viewer-modal.component';
import { ListComponentComponent } from './list-component/list-component.component';
import { TabComponentComponent } from './tab-component/tab-component.component';
import { TabItemComponent } from './tab-component/tab-item/tab-item.component';
import { LeafletGlobalPositionsComponent } from './leaflet-global-positions/leaflet-global-positions.component';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { LeafletMarkerClusterModule } from '@asymmetrik/ngx-leaflet-markercluster';
import { NgxPaginationModule } from 'ngx-pagination';

@NgModule({
  declarations: [
    NavigationMenuComponent,
    NavigationMenuItemComponent,
    IconComponent,
    AugmentedCardComponent,
    AnimatedHeaderComponent,
    LoadingPanelComponent,
    GlitchTextComponent,
    JsonObjectViewerComponent,
    PopupModalComponent,
    JsonObjectViewerModalComponent,
    ListComponentComponent,
    TabComponentComponent,
    TabItemComponent,
    LeafletGlobalPositionsComponent,
    
  ],
  imports: [
    CommonModule,
    RouterModule,
    LeafletModule,
    LeafletMarkerClusterModule,
    NgxPaginationModule,
  ],
  entryComponents: [
    PopupModalComponent
  ],
  exports: [
    NavigationMenuComponent,
    IconComponent,
    AugmentedCardComponent,
    AnimatedHeaderComponent,
    LoadingPanelComponent,
    GlitchTextComponent,
    JsonObjectViewerComponent,
    ListComponentComponent,
    TabComponentComponent,
    TabItemComponent,
    LeafletGlobalPositionsComponent
  ]
})
export class SharedModule { }
