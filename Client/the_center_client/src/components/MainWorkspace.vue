<template>
    <Button @click="getboard()">刷新</Button>
    <Row>
        <Col v-for="item in list" :key="item.id" :span="item.w">
            <Card title="c">{{ item.cName }}</Card>
        </Col>
    </Row>
</template>

<script lang="ts">
import { defineComponent, onMounted, onUnmounted, ref } from "vue";
import { Row, Col, Button, Card } from "ant-design-vue";
import { Board, GetBoards, onConnected } from "../api/workspace";

export default defineComponent({
    components: {
        Row,
        Col,
        Button,
        Card,
    },
    props: {
        workspace: {
            type: String,
            validator(this: void, v: string) {
                return !!v;
            },
            required: true,
        },
    },
    setup: (prop) => {
        const list = ref([] as Board[]);
        const getboard = async () => {
            list.value = await GetBoards(prop.workspace);
        };
        onConnected(() => getboard());

        return { getboard, list };
    },
});
</script>

<style></style>
