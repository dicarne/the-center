<template>
    <div class="main-work-area">
        <a-button @click="getboard()">刷新</a-button>
    </div>

    <Row :gutter="[16, 16]">
        <Col :span="6">
            <div class="card-board card-body">
                <a-button @click="createBoard">+</a-button>
            </div>
        </Col>
        <Col v-for="item in list" :key="item.id" :span="item.w">
            <BoardCard
                :ver="item.ver"
                :u-i-coms="item.uIComs"
                :workspace="workspace"
                :boardid="item.id"
                :getboard="getboard"
            />
        </Col>
    </Row>
    <a-modal title="增加新卡片" v-model:visible="newcard_visiable" @ok="newcard_ok">
        <a-menu v-model:selectedKeys="createCard.select" @click="createCard.click">
            <a-menu-item :key="item" v-for="item in createCard.cardTypes">
                <p>{{ item }}</p>
            </a-menu-item>
        </a-menu>
    </a-modal>
</template>

<script lang="ts">
import { defineComponent, onMounted, onUnmounted, reactive, ref } from "vue";
import { Row, Col } from "ant-design-vue";
import { BoardUI, CreateBoard, FocusWorkspace, GetBoards, onConnected } from "../api/workspace";
import BoardElement from "../components/BoardElement.vue"
import BoardCard from "./BoardCard.vue"
import { workspaces } from "../connection/Server"

export default defineComponent({
    components: {
        Row,
        Col,
        BoardElement,
        BoardCard
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
        const list = ref([] as BoardUI[]);

        const getboard = async () => {
            list.value = await GetBoards(prop.workspace);
            list.value.forEach(u => u.ver = 0)
        };
        onConnected(async () => {
            try {
                await FocusWorkspace(prop.workspace)
                await getboard()
            } catch (error) {
                console.error(error)
            }

        });

        const dispatchBoard = (board: string, data: string) => {
            let jdata = JSON.parse(data)
            let index = list.value.findIndex(u => u.id === board)
            list.value[index].uIComs = ref(jdata)
            list.value[index].ver++
        }
        onMounted(() => {
            if (prop.workspace != "home") {
                workspaces.set(prop.workspace, dispatchBoard)
            }
        })
        onUnmounted(() => {
            workspaces.delete(prop.workspace)
        })

        // 创建卡片
        const createCard = reactive({
            cardTypes: [] as string[],
            select: [] as string[],
            click: (e: any) => {

            }
        })
        const newcard_visiable = ref(false)
        const newcard_ok = async () => {
            if (createCard.select.length != 0) {
                await CreateBoard(prop.workspace, createCard.select[0])
                await getboard()
                newcard_visiable.value = false
            }

        }
        const createBoard = async () => {
            newcard_visiable.value = true
            createCard.cardTypes = ["runscript"]
        }
        // ------
        return { getboard, list, createBoard, workspace: prop.workspace, newcard_visiable, newcard_ok, createCard };
    },
});
</script>

<style>
.main-work-area {
    padding-bottom: 10px;
}
.card-body {
    background-color: #fff;
    padding: 24px;
}
.card-board {
    border-radius: 8px;
    box-shadow: 0px 0px 10px #e6e6e6;
    position: relative;
}
.card-board:hover {
    box-shadow: 0px 0px 10px #d3d3d3;
}
</style>
