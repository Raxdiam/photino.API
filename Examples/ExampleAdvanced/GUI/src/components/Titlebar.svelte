<script lang="ts">
  import { photino } from '$stores';
  import { onMount } from 'svelte';

  import icon from '$assets/photino-logo.png';

  const defaultStyles = { 
    titlebarBgActive: '#3C3C3C', 
    titlebarBgInactive: '#323233',
    titlebarTextActive: '#CCCCCC',
    titlebarTextInactive: '#8E8E8E',
    titlebarCtrlActive: '#CACACA',
    titlebarCtrlInactive: '#8C8C8D'
  }
  const styles: { titlebarBg?: string, titlebarText?: string, titlebarCtrl?: string } = {};
  
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
  };

  $photino.window.on('focusIn', () => { 
    styles.titlebarBg = defaultStyles.titlebarBgActive; 
    styles.titlebarText = defaultStyles.titlebarTextActive;
    styles.titlebarCtrl = defaultStyles.titlebarCtrlActive;
  });
  $photino.window.on('focusOut', () => { 
    styles.titlebarBg = defaultStyles.titlebarBgInactive; 
    styles.titlebarText = defaultStyles.titlebarTextInactive;
    styles.titlebarCtrl = defaultStyles.titlebarCtrlInactive;
  });
  $photino.window.on('maximized', () => maxresIcon = 'codicon-chrome-restore');
  $photino.window.on('restored', () => maxresIcon = 'codicon-chrome-maximize');

  onMount(async () => {
    title = await $photino.window.getTitle();    
    maxresIcon = (await $photino.window.getMaximized()) ? 'codicon-chrome-restore' : 'codicon-chrome-maximize';
  });
</script>

<div class="window-titlebar" style="background-color: {styles.titlebarBg}; color: {styles.titlebarText};">
  <div class="window-title">{title}</div>

  <div>
    <div class="window-icon"><img src={icon} alt=""></div>
    <div class="window-toolbar" />
  </div>

  <div on:mousedown={handleDrag} class="window-drag-area" />

  <div class="window-buttons" style="color: {styles.titlebarCtrl} !important;">
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

    .window-icon {
      display: flex;
      justify-content: center;
      align-items: center;
      width: 35px;
      height: 30px;

      & > img {
        width: 22px;
      }
    }

    .window-drag-area {
      width: 100%;
      z-index: 2;
    }

    .window-buttons {
      display: flex;
      align-self: flex-end;
      z-index: 2;
      //color: #cacaca;

      .window-button {        
        width: 46px;
        line-height: 30px;

        &:hover {
          background-color: #505050;
        }

        &.window-close:hover {
          background-color: #d71526;
        }
      }
    }
  }
</style>
