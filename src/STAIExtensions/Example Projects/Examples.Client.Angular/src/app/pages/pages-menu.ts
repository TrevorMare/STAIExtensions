export interface IMenuItem {
    link?: string;
    icon?: string;
    iconSet?: string;
    title?: string;
}

export const MENU_ITEMS : IMenuItem[] = [
    {
        title: 'Home',
        icon: 'donut_large',
        iconSet: 'material-icons',
        link: '/pages/home-page'
    },
    {
        title: 'Telemetry Overview',
        icon: 'donut_large',
        iconSet: 'material-icons',
        link: '/pages/telemetry-overview'
    },
    {
        title: 'Availability',
        icon: 'fas fa-heartbeat',
        iconSet: 'font-awesome',
        link: '/pages/availability-overview'
    },
    {
        title: 'Browser Timings',
        icon: 'far fa-clock',
        iconSet: 'font-awesome',
        link: '/pages/browser-timing-overview'
    },
    {
        title: 'Custom Events',
        icon: 'playlist_add',
        iconSet: 'material-icons',
        link: '/pages/custom-events-overview'
    },
    {
        title: 'Custom Metrics',
        icon: 'assessment',
        iconSet: 'material-icons',
        link: '/pages/home-page4'
    },
    {
        title: 'Dependencies',
        icon: 'merge_type',
        iconSet: 'material-icons',
        link: '/pages/home-page5'
    },
    {
        title: 'Exceptions',
        icon: 'error_outline',
        iconSet: 'material-icons',
        link: '/pages/home-page6'
    },
    {
        title: 'Page Views',
        icon: 'far fa-eye',
        iconSet: 'font-awesome',
        link: '/pages/home-page7'
    },
    {
        title: 'Performance Counters',
        icon: 'fas fa-chart-bar',
        iconSet: 'font-awesome',
        link: '/pages/home-page8'
    },
    {
        title: 'Requests',
        icon: 'devices',
        iconSet: 'material-icons',
        link: '/pages/home-page9'
    },
    {
        title: 'Traces',
        icon: 'text_fields',
        iconSet: 'material-icons',
        link: '/pages/traces-overview'
    },
];