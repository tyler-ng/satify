<template>
  <div class="inventory-container">
    <h1 id="inventoryTitle">Inventory Dashboard</h1>
    <hr />

    <div class="inventory-action">
      <satify-button @button:click="showNewProductModal" id="addNewBtn">
        Add New Item
      </satify-button>
      <satify-button @button:click="showShipmentModal" id="receiveShipmentBtn">
        Receive Shipment
      </satify-button>
    </div>

    <table id="inventoryTable" class="table">
      <tr>
        <th>Item</th>
        <th>Quantity On-hand (bags)</th>
        <th>Ideal Quantity (bags)</th>
        <th>Unit Price</th>
        <th>Created On</th>
        <th>Edit</th>
        <th>Delete</th>
      </tr>
      <tr v-for="item in inventory" :key="item.id">
        <td>
          {{ item.product.name }}
        </td>
        <td
          v-bind:class="`${applyColor(
            item.quantityOnHand,
            item.idealQuantity
          )}`"
        >
          {{ item.quantityOnHand }}
        </td>
        <td>
          {{ item.product.idealQuantity }}
        </td>
        <td>
          {{ item.product.price | money }}
        </td>
        <td>
          {{ item.product.createdOn | humanizeDate }}
        </td>
        <td>
          <div
            class="lni-pencil product-edit"
            @click="showEditProductModal(item.product.id)"
          ></div>
        </td>
        <td>
          <div
            class="lni-cross-circle product-archive"
            @click="showDeleteProductModal(item.product.id)"
          ></div>
        </td>
      </tr>
    </table>
    <new-product-modal
      v-if="isNewProductVisible"
      @save:product="saveNewProduct"
      @close="closeModals"
    />
    <shipment-modal
      v-if="isShipmentVisible"
      :inventory="inventory"
      @save:shipment="saveNewShipment"
      @close="closeModals"
    />

    <edit-product-modal
      v-if="isEditProductVisible"
      :edit-product="product"
      @save:edit-product="saveEditedProduct"
      @close="closeModals"
    />
    <delete-product-modal
      v-if="isDeleteProductVisible"
      :delete-product="product"
      @delete:product="deleteProduct"
      @close="closeModals"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IProduct, IProductInventory } from "@/types/Product";
import SatifyButton from "@/components/SatifyButton.vue";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import { IShipment } from "@/types/Shipment";
import { InventoryService } from "@/services/inventory-service";
import { ProductService } from "@/services/product-service";
import EditProductModal from "@/components/modals/EditProductModal.vue";
import DeleteProductModal from "@/components/modals/DeleteProductModal.vue";

const inventoryService = new InventoryService();
const productService = new ProductService();

@Component({
  name: "Inventory",
  components: {
    SatifyButton,
    NewProductModal,
    EditProductModal,
    DeleteProductModal,
    ShipmentModal,
  },
})
export default class Inventory extends Vue {
  isNewProductVisible: boolean = false;
  isEditProductVisible: boolean = false;
  isShipmentVisible: boolean = false;
  isDeleteProductVisible: boolean = false;

  inventory: IProductInventory[] = [];
  product: IProduct = {
    createdOn: new Date(),
    updatedOn: new Date(),
    id: 0,
    description: "",
    isTaxable: false,
    name: "",
    price: 0,
    isArchived: false,
    idealQuantity: 0,
  };

  applyColor(current: number, target: number) {
    if (current <= 0) {
      return "red";
    }

    if (Math.abs(target - current) > 8) {
      return "yellow";
    }

    return "green";
  }

  async closeModals() {
    this.isNewProductVisible = false;
    this.isShipmentVisible = false;
    this.isEditProductVisible = false;
    this.isDeleteProductVisible = false;
    await this.initialize();
  }

  showNewProductModal() {
    this.isNewProductVisible = true;
  }

  showEditProductModal(productId: number) {
    this.isEditProductVisible = true;
    this.product = this.inventory.find(
      (item) => item.id === productId
    )!.product;
  }

  showShipmentModal() {
    this.isShipmentVisible = true;
  }

  async saveEditedProduct(editProduct: IProduct) {
    await productService.edit(editProduct);
    this.isEditProductVisible = false;
    await this.initialize();
  }

  async showDeleteProductModal(productId: number) {
    this.isDeleteProductVisible = true;
    this.product = this.inventory.find(
      (item) => item.id === productId
    )!.product;
  }

  async saveNewProduct(newProduct: IProduct) {
    await productService.save(newProduct);
    this.isNewProductVisible = false;
    await this.initialize();
  }

  async saveNewShipment(shipment: IShipment) {
    await inventoryService.updateInventoryQuantity(shipment);
    this.isShipmentVisible = false;
    await this.initialize();
  }

  async deleteProduct() {
    await productService.archive(this.product.id);
    await this.initialize();
    this.isDeleteProductVisible = false;
  }

  async initialize() {
    this.inventory = await inventoryService.getInventory();
  }

  async created() {
    await this.initialize();
  }
}
</script>
<style scoped lang="scss">
@import "@/scss/global.scss";

.green {
  font-weight: bold;
  color: $satify-green;
}

.yellow {
  font-weight: bold;
  color: $satify-yellow;
}

.red {
  font-weight: bold;
  color: $satify-red;
}

.inventory-action {
  display: flex;
  margin-bottom: 0.8rem;
}

.product-archive {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $satify-red;
}

.product-edit {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $satify-green;
}
</style>
