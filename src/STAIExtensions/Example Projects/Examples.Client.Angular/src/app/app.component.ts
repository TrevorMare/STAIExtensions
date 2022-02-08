import { Component } from '@angular/core';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router, RouterEvent } from '@angular/router';
import { MENU_ITEMS } from './pages/pages-menu';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'STAIExtensions UI';
  isMenuOpen: boolean = true;
  isBusyNavigating: boolean = true;

  public menuItems = MENU_ITEMS;
  
  constructor(private router: Router) {
    router.events.subscribe((event: any) => {
      this.navigationInterceptor(event)
    })
  }
  
  navigationInterceptor(event: RouterEvent): void {
    if (event instanceof NavigationStart) {
      this.isBusyNavigating = true;
    }
    if (event instanceof NavigationEnd) {
      this.isBusyNavigating = false;
    }

    // Set loading state to false in both of the below events to hide the spinner in case a request fails
    if (event instanceof NavigationCancel) {
      this.isBusyNavigating = false;
    }
    if (event instanceof NavigationError) {
      this.isBusyNavigating = false;
    }
  }

  toggleMenuOpen() {
    this.isMenuOpen = !this.isMenuOpen;
  }

}
