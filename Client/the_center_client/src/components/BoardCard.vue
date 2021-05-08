<template></template>
<script lang="ts">
import { defineComponent } from "vue";
import BoardElement from "./BoardElement.vue"

export default defineComponent({
    components: {
        BoardElement
    },
    props: {
        workspace: {
            type: String,
            required: true,
        },
        boardid: {
            type: String,
            required: true,
        },
    },
    setup: (prop) => {
        
    },
})
</script>
<style>
</style>