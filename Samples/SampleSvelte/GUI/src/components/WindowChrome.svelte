<script lang="ts">
  import { Photino } from 'photino.js';
  import { onMount } from 'svelte';

  export let visible: boolean = true;
  export let title: string = '';

  onMount(async () => {
    title = await Photino.window.getTitle();
  });
</script>

{#if visible}
  <div class="chrome">
    <div class="chrome-titlebar blur">
      <div class="chrome-titlebar-drag" />
      <div class="chrome-titlebar-text">
        {#if title}
          {title}
        {/if}
      </div>
      <div class="chrome-titlebar-controls">
        <div class="chrome-titlebar-controls-minimize" />
        <div class="chrome-titlebar-controls-maximize" />
        <div class="chrome-titlebar-controls-close" />
      </div>
    </div>

    <div class="chrome-userarea">
      <slot />
    </div>
  </div>
{:else}
  <slot />
{/if}

<style lang="scss">
  .chrome {
    position: absolute;
    top: 0;
    left: 0;

    display: flex;
    flex-direction: column;
    width: 100vw;
    height: 100vh;
    padding: 1px;

    .chrome-titlebar {
      position: relative;
      display: flex;
      height: 30px;
      background: #1c202b;
      color: #fff;
      flex-shrink: 0;
      justify-content: center;
      align-items: center;
      user-select: none;
      -webkit-user-select: none;

      &.blur {
        color: #797979;
      }

      .chrome-titlebar-drag {
        position: absolute;
        top: 0;
        left: 0;
        display: block;
        width: 100%;
        height: 100%;
      }

      .chrome-titlebar-text {
        position: absolute;
        left: 50%;
        transform: translate(-50%, 0px);
        max-width: calc(100vw - 296px);
        flex: 0 1 auto;
        font-size: 12px;
        overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
        margin-left: auto;
        margin-right: auto;
      }

      .chrome-titlebar-controls {
        display: flex;
        flex-grow: 0;
        flex-shrink: 0;
        z-index: 3000;
      }
    }

    .chrome-userarea {
      padding: 8px;
    }
  }
</style>
