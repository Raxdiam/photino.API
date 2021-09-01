<script lang="ts">
  import type { ViewList } from '$lib/ViewList';
  import type { ActionList } from '$lib/ActionList';
  import { ResizeFrame, Titlebar, ViewContainer } from '$components';
  import { HomeView, TestView } from '$components/views';

  const views: ViewList = [
    { id: 'home', name: 'Home', iconClass: 'codicon codicon-home', component: HomeView },
    { id: 'test', name: 'Test', iconClass: 'codicon codicon-beaker', component: TestView },
  ];

  const actions: ActionList = [
    { id: 'settings', name: 'Settings', iconClass: 'codicon codicon-settings-gear', run: () => { console.log('Settings action button clicked') } }
  ];
</script>

<ResizeFrame />
<div id="app">
  <Titlebar />
  <ViewContainer {views} {actions} />
</div>

<style lang="scss">
  #app {
    display: flex;
    flex-direction: column;
    height: 100vh;
  }
</style>
