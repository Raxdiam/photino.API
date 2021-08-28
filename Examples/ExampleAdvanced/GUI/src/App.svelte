<script lang="ts">
  import ResizeFrame from '$components/ResizeFrame.svelte';
  import Titlebar from '$components/Titlebar.svelte';
  import { photino } from '$stores';
</script>

<ResizeFrame />
<Titlebar />
<div id="window-content" />

<style lang="scss">
</style>
