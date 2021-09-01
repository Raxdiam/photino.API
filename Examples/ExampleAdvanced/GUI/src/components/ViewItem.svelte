<script lang="ts">
  import { createEventDispatcher } from 'svelte';

  import type { ViewListItem } from '$lib/ViewList';

  export let view: ViewListItem;
  export const setActive = (value: boolean) => (active = value);
  
  const dispatch = createEventDispatcher();

  let active: boolean;

  function handleClick() {
    dispatch('active', view);
  }
</script>

<div on:click={handleClick} class="{active ? 'checked ' : ''}{view.iconClass} action-item" />

<style lang="scss">
  $size: 48px;

  .action-item {
    display: flex;
    height: $size;
    justify-content: center;
    align-items: center;
    font-size: 26px;
    color: rgba($color: #fff, $alpha: 0.4);
    border-left: 2px solid transparent;
    border-right: 2px solid transparent;
    cursor: pointer;

    &.checked {
      color: #fff;
      border-left-color: #fff;
    }

    &:hover {
      color: #fff;
    }

    /* &:active {
      border-left-color: #007fd4;
    } */
  }
</style>
