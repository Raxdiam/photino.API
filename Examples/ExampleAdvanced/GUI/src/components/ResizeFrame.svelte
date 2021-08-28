<script lang="ts">
  import { photino } from '$stores';

  function handleResize(e: MouseEvent) {
    const el = e.target as HTMLElement;
    $photino.window.resize(el.dataset.code);
    e.preventDefault();
    e.stopPropagation();
  }
</script>

<div class="resize-frame">
  <div class="resize-frame-top">
    <div on:mousedown={handleResize} class="resize-handle corner nw" data-code="tl" />
    <div on:mousedown={handleResize} class="resize-handle hbar n" data-code="t" />
    <div on:mousedown={handleResize} class="resize-handle corner ne" data-code="tr" />
  </div>

  <div class="resize-frame-mid">
    <div on:mousedown={handleResize} class="resize-handle vbar w" data-code="l" />
    <div on:mousedown={handleResize} class="resize-handle vbar e" data-code="r" />
  </div>

  <div class="resize-frame-bottom">
    <div on:mousedown={handleResize} class="resize-handle corner sw" data-code="bl" />
    <div on:mousedown={handleResize} class="resize-handle hbar s" data-code="b" />
    <div on:mousedown={handleResize} class="resize-handle corner se" data-code="br" />
  </div>
</div>

<style lang="scss">
  $handle-size: 6px;

  .resize-frame {
    display: flex;
    flex-direction: column;
    position: absolute;
    width: 100vw;
    height: 100vh;
    z-index: 3;
    pointer-events: none;

    .resize-frame-top {
      display: flex;
      width: 100%;
    }

    .resize-frame-mid {
      display: flex;
      justify-content: space-between;
      height: 100%;
    }

    .resize-frame-bottom {
      display: flex;
      width: 100%;
    }

    .resize-handle {
      pointer-events: all;

      &.corner {
        width: $handle-size;
        height: $handle-size;

        &.nw { cursor: nw-resize; }
        &.ne { cursor: ne-resize; }
        &.sw { cursor: sw-resize; }
        &.se { cursor: se-resize; }
      }

      &.hbar {
        height: $handle-size;
        width: 100%;

        &.n { cursor: n-resize; }
        &.s { cursor: s-resize; }
      }

      &.vbar {
        width: $handle-size;

        &.w { cursor: w-resize; }
        &.e { cursor: e-resize; }
      }
    }
  }
</style>
