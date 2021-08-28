<script lang="ts">
  import { photino } from '$stores';

  function handleResize(e: MouseEvent) {
    const el = e.target as HTMLElement;
    $photino.window.resize(el.dataset.resize);
    e.preventDefault();
    e.stopPropagation();
  }

  function handleDrag(e: MouseEvent) {
    $photino.window.drag();
    e.preventDefault();
    e.stopPropagation();
  }
</script>

<div id="window-controls">
  <div class="resize-handles">
    <div class="row">
      <div on:mousedown={handleResize} class="resize" style="cursor: nw-resize" data-resize="tl" />
      <div on:mousedown={handleResize} class="resize" style="cursor: n-resize" data-resize="t" />
      <div on:mousedown={handleResize} class="resize" style="cursor: ne-resize" data-resize="tr" />
    </div>

    <div class="row">
      <div on:mousedown={handleResize} class="resize" style="cursor: w-resize" data-resize="l" />
      <div on:mousedown={handleResize} class="resize blank" />
      <div on:mousedown={handleResize} class="resize" style="cursor: e-resize" data-resize="r" />
    </div>

    <div class="row">
      <div on:mousedown={handleResize} class="resize" style="cursor: sw-resize" data-resize="bl" />
      <div on:mousedown={handleResize} class="resize" style="cursor: s-resize" data-resize="b" />
      <div on:mousedown={handleResize} class="resize" style="cursor: se-resize" data-resize="br" />
    </div>
  </div>

  <div on:mousedown={handleDrag} id="drag-area">Drag Me</div>

  <button on:click={() => $photino.window.setMinimized(true)}>Minimize</button>
  <button on:click={() => $photino.window.setMaximized(true)}>Maximize</button>
  <button on:click={() => $photino.window.setMaximized(false)}>Restore</button>
  <button on:click={() => $photino.window.close()}>Close</button>
</div>

<style lang="scss">
  #window-controls {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 0.5rem;
  }

  .resize-handles {
    display: flex;
    flex-direction: column;
    gap: 0.25rem;

    .row {
      display: flex;
      flex-direction: row;
      gap: 0.25rem;

      .resize {
        width: 1.5rem;
        height: 1.5rem;
        color: gray;
        background: gainsboro;

        &.blank {
          background: none;
        }
      }
    }
  }

  #drag-area {
    display: flex;
    color: gray;
    background: gainsboro;
    width: 5rem;
    height: 100%;
    justify-content: center;
    align-items: center;
    line-height: 1rem;
    -webkit-user-select: none;
  }
</style>
