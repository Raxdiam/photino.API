<script lang="ts">
  import TestButton from '$components/TestButton.svelte';
</script>

<main>
  <TestButton text="Current Directory" defaultMessage="Click the button" ns="io" action="cwd" />
  <TestButton
    text="Open File"
    defaultMessage="Click the button"
    ns="os"
    action="showOpenFileDialog"
    params={['Open file', false, { name: 'All files', patterns: ['*.*'] }]}
    messageProperty="path"
  />
</main>

<style lang="scss">
</style>
