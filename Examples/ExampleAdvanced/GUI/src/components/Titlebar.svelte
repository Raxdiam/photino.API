<script lang="ts">
  import { photino } from '$stores';
  import { onMount } from 'svelte';

  //TODO: Implement Maximized event on backend and subscribe here to reset the maximize/restore button

  let title: string;
  let maxresIcon: string;

  function handleDrag(e: MouseEvent) {
    $photino.window.drag();
    e.preventDefault();
    e.stopPropagation();
  }

  const handleMinimize = async (e: MouseEvent) => {
    const el = e.target as HTMLElement;
    const parent = el.parentElement;
    parent.removeChild(el);
    await $photino.window.setMinimized(true);
    parent.insertBefore(el, parent.firstChild);
  };
  const handleClose = () => $photino.window.close();
  const handleMaximizeRestore = async () => {
    const isMaximized = await $photino.window.getMaximized();
    await $photino.window.setMaximized(!isMaximized);
    maxresIcon = !isMaximized ? 'codicon-chrome-restore' : 'codicon-chrome-maximize';
  };

  onMount(async () => {
    title = await $photino.window.getTitle();
    maxresIcon = (await $photino.window.getMaximized()) ? 'codicon-chrome-restore' : 'codicon-chrome-maximize';
  });
</script>

<div class="window-titlebar">
  <div class="window-title">{title}</div>

  <div>
    <div class="window-icon" />
    <div class="window-toolbar" />
  </div>

  <div on:mousedown={handleDrag} class="window-drag-area"></div>

  <div class="window-buttons">
    <div on:click={handleMinimize} class="window-button codicon codicon-chrome-minimize" />
    <div on:click={handleMaximizeRestore} class="window-button codicon {maxresIcon}" />
    <div on:click={handleClose} class="window-button window-close codicon codicon-chrome-close" />
  </div>
</div>

<style lang="scss">
  .window-titlebar {
    display: flex;
    justify-content: space-between;
    color: #cccccc;
    background: #3c3c3c;
    height: 30px;
    -webkit-user-select: none;

    .window-title {
      display: flex;
      position: absolute;
      z-index: 0;
      width: 100vw;
      height: 30px;
      font-size: 12px;
      align-items: center;
      justify-content: center;
    }

    .window-drag-area {
      width: 100%;
      z-index: 2;
    }

    .window-buttons {
      display: flex;
      align-self: flex-end;
      z-index: 2;

      .window-button {
        color: #cacaca;
        width: 46px;
        line-height: 30px;

        &:hover {
          background-color: #505050;
        }

        &:focus, &:active {
          background-color: #3c3c3c;
        }

        &.window-close:hover {
          background-color: #d71526;
        }
      }
    }
  }
</style>
