<template>
  <satify-modal>
    <template v-slot:header> Delete Product </template>
    <template v-slot:body>
      <ul class="deleteProduct">
        <li>
          Product: <strong>{{ deleteProduct.name }}</strong>
        </li>
        <li><br /></li>
        <li>Do you want to delete this item?</li>
      </ul>
    </template>
    <template v-slot:footer>
      <satify-button
        type="button"
        @click.native="remove"
        aria-label="delete item"
      >
        Yes
      </satify-button>
      <satify-button
        type="button"
        @click.native="close"
        aria-label="close modal"
      >
        No
      </satify-button>
    </template>
  </satify-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import { IProduct } from "@/types/Product";
import SatifyButton from "@/components/SatifyButton.vue";
import SatifyModal from "@/components/modals/SatifyModal.vue";

@Component({
  name: "DeleteProductModal",
  components: { SatifyButton, SatifyModal },
})
export default class DeleteProductModal extends Vue {
  @Prop({ required: true, type: Object as () => IProduct })
  deleteProduct: IProduct;

  close() {
    this.$emit("close");
  }

  remove() {
    this.$emit("delete:product", this.deleteProduct);
  }
}
</script>

<style scoped></style>
