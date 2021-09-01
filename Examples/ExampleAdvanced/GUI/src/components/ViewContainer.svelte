<script lang="ts">
  import { onMount } from 'svelte';
  import { viewList, activeView } from '$stores';
  import type { ViewList, ViewListItem } from '$lib/ViewList';
  import type { ActionList, ActionListItem } from '$lib/ActionList';
  import { ViewItem } from '$components';
  import ActionItem from './ActionItem.svelte';

  export let views: ViewList;
  export let actions: ActionList;

  $viewList = views;

  function setActiveView(view: ViewListItem) {
    view.item.setActive(true);
    $activeView = view;
  }

  function handleViewItemActive(e: CustomEvent<ViewListItem>) {
    for (const view of views) {
      view.item.setActive(false);
    }
    setActiveView(e.detail);
  }

  onMount(() => {
    if ($activeView && $activeView.item) {
      $activeView.item.setActive(true);
    } else {
      setActiveView(views[0]);
    }
  });
</script>

<div class="view-container">
  <div class="action-bar">
    <ul class="actions-container">
      {#each views as view}
        <ViewItem on:active={handleViewItemActive} bind:this={view.item} {view} />
      {/each}
    </ul>

    <ul class="actions-container">
      {#each actions as action}
        <ActionItem bind:this={action.item} {action} />
      {/each}
    </ul>
  </div>

  <div class="view-content">
    {#if $activeView?.component}
      <svelte:component this={$activeView.component} />
    {/if}
  </div>
</div>

<style lang="scss">
  .view-container {
    display: flex;
    flex-direction: row;
    height: 100%;
    width: 100%;

    .action-bar {
      $size: 48px;

      display: flex;
      flex-direction: column;
      flex-shrink: 0;
      justify-content: space-between;
      width: $size;
      background-color: #333333;

      .actions-container {
        display: flex;
        flex-direction: column;
      }
    }

    .view-content {
      width: 100%;
      height: 100%;
    }
  }
</style>
