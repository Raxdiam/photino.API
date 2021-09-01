<script lang="ts">
  import type { ActionListItem } from '$lib/ActionList';

  export let action: ActionListItem;
</script>

<div on:click={action.run} class="action-item {action.iconClass}" />

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

    &:hover {
      color: #fff;
    }
  }
</style>
